# ltly - Simple and Comfortable URL Shortener

[Website](https://ltly.dbrdak.com) | [Chrome Extension](https://chromewebstore.google.com/detail/ltly-url-shortener/cedkbgiolaniknhlhieibefijcmjckkg) | [Postman Collection](https://github.com/DBrdak/ltly/blob/master/ltly.postman_collection.json)

ltly is a cloud-native URL shortener designed for simplicity and ease-of-use. It provides two endpoints—one to shorten URLs and another to redirect shortened URLs. Leveraging DynamoDB to store the URLs, ltly is accessible via a website, an API, and a Chrome extension. Shortened URLs are served from a personal domain with a `/s` suffix.

---

## Table of Contents

- [Features](#features)
- [Tech Stack](#tech-stack)
- [Installation and Setup](#installation-and-setup)
- [Usage](#usage)
- [Configuration](#configuration)
- [Contributing](#contributing)
- [License](#license)

---

## Features

- **Simple URL Shortening:**  
  Easily shorten any URL using a straightforward API endpoint.

- **Redirection:**  
  Redirect users from a short URL to the original URL seamlessly.

- **Multiple Access Points:**  
  Use the website, API, or Chrome extension to create and manage shortened URLs.

- **Cloud-Native Architecture:**  
  Built entirely on AWS cloud services for scalability and reliability.

---

## Tech Stack

### Backend

- **Language & Framework:**  
  C# + .NET

### Frontend

- **Framework & Libraries:**  
  React, Typescript, Vite

### Cloud Services

- **Database:**  
  DynamoDB
  
- **Serverless Functions:**  
  AWS Lambda
  
- **API Management:**  
  API Gateway
  
- **Infrastructure Management:**  
  CloudFormation, EventBridge

---

## Installation and Setup

This project is completely cloud-native. To set it up, follow these instructions:

### Backend

- **Deployment:**  
  Download the code and deploy it as a development environment using the provided `template.yml` file.

- **Local Testing:**  
  Alternatively, use the AWS Lambda Mock Tool for .NET to test functions locally.

### Frontend

1. Navigate to the `Ltly.WebClient` directory:

    ```bash
    cd Ltly.WebClient
    ```

2. Run the development server:

    ```bash
    npm run dev
    ```

---

## Usage

- **Shortening URLs:**  
  Use the designated endpoint to shorten URLs. The service will generate a short URL with a `/s` suffix based on your personal domain.

- **Redirection:**  
  When a user visits a shortened URL, the service will redirect them to the original URL.

- **Multiple Access Points:**  
  Create and manage shortened URLs using the website, API, or Chrome extension.

---

## Configuration

For deployment, ensure that all necessary environment variables and configuration settings are properly set. Configuration management is primarily handled through AWS CloudFormation templates defined in the `template.yml` file. Make sure to update any configuration details as required for your AWS environment.

---

## Contributing

Contributions are welcome! If you would like to enhance or modify any part of ltly, feel free to clone the repository and submit pull requests. Your contributions are greatly appreciated.

---

## License

This project is open for public use. *(Feel free to add any additional licensing details if necessary.)*
