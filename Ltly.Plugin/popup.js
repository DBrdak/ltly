document.addEventListener('DOMContentLoaded', function() {
  const shortenBtn = document.getElementById('shorten-btn');
  const urlInput = document.getElementById('url-input');
  const resultDiv = document.getElementById('result');

  shortenBtn.addEventListener('click', function() {
    let url = urlInput.value;

    if (!url) {
      chrome.tabs.query({ active: true, currentWindow: true }, function(tabs) {
        url = tabs[0].url;
        shortenUrl(url);
      });
    } else {
      shortenUrl(url);
    }
  });

  function shortenUrl(url) {
    if (!isValidUrl(url)) {
      resultDiv.textContent = 'Please enter a valid URL.';
      return;
    }

    const apiUrl = `https://dbrdak.com/s?url=${encodeURIComponent(url)}`;

    fetch(apiUrl, { method: 'POST' })
      .then(response => response.json())
      .then(data => {
        if (data.value.shortenedValue) {
          const shortUrl = data.value.shortenedValue;
          resultDiv.innerHTML = `<a href="${shortUrl}" target="_blank">${shortUrl}</a><br>(Copied to clipboard)`;

          copyToClipboard(shortUrl);
        } else {
          resultDiv.textContent = 'Error: Unable to shorten the URL.';
        }
      })
      .catch(error => {
        console.error('Error:', error);

        if (error.status === 400) {
          resultDiv.textContent = 'Error: Unable to connect to the API.';
        } else {
          resultDiv.textContent = 'Error: Unable to connect to the API.';
        }
      });
  }

  function copyToClipboard(text) {
    navigator.clipboard.writeText(text).then(() => {
      console.log('Shortened URL copied to clipboard.');
    }).catch(err => {
      console.error('Could not copy text: ', err);
    });
  }

  function isValidUrl(string) {
    try {
      new URL(string);
      return true;
    } catch (_) {
      return false;
    }
  }
});
