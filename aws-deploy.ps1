$ltlyStackName = "ltly"
$ltlyTemplate = ".\Ltly.Lambda\template.yaml"
$ltlyPackagedTemplate = ".\Ltly.Lambda\packaged-template.yaml"

$preStackName = "ltly-pre"
$preTemplate = ".\pre-template.yaml"

$awsProfile = "dbrdak-lambda"
$region = "eu-central-1"

$acmCertArn = aws acm list-certificates --query "CertificateSummaryList[?DomainName=='ltly.link'].CertificateArn" --output text

aws cloudformation deploy `
    --template-file $preTemplate `
    --stack-name $preStackName `
    --capabilities CAPABILITY_NAMED_IAM `
    --region $region `
    --profile $awsProfile

$s3bucket = (aws cloudformation describe-stacks `
    --stack-name $preStackName `
    --query "Stacks[0].Outputs[?OutputKey=='S3BucketName'].OutputValue" `
    --output text `
    --region $region `
    --profile $awsProfile)

if (-not $s3bucket) {
    Write-Host "Error: Could not retrieve the Name of the S3 bucket. Exiting."
    exit 1
}

sam build --template $ltlyTemplate
sam package --s3-bucket $s3bucket --output-template-file $ltlyPackagedTemplate

sam deploy --template-file $ltlyPackagedTemplate `
    --stack-name $ltlyStackName `
    --capabilities CAPABILITY_NAMED_IAM `
    --parameter-overrides "ACMCertificateArn=$acmCertArn" `
    --region $region `
    --profile $awsProfile
    
$url = (aws cloudformation describe-stacks `
--stack-name $ltlyStackName `
--query "Stacks[0].Outputs[?OutputKey=='ApiURL'].OutputValue" `
--output text `
--region $region `
--profile $awsProfile)

Write-Host "Successfully deployed" -ForegroundColor Blue
Write-Host "API URL: $url" -ForegroundColor Green