import { useState, useEffect } from 'react'
import './App.css'

function App() {
  const [url, setUrl] = useState('');
  const [valid, setValid] = useState(false);
  const resultDiv = document.getElementById('result');

  const validateUrl = (text) => {
    const urlRegex = /^(https?:\/\/)?([a-zA-Z0-9.-]+)\.([a-zA-Z]{2,6})(\/\S*)?$/;
    return urlRegex.test(text);
  };

  const handleUrlChange = (input) => {
    setUrl(input);
    setValid(validateUrl(input));
  };

  function copyToClipboard(text) {
    navigator.clipboard.writeText(text).then(() => {
      console.log('Shortened URL copied to clipboard.');
    }).catch(err => {
      console.error('Could not copy text: ', err);
    });
  }

  const shortenUrl = async (e) => {
    e.preventDefault();
    fetch(`https://dbrdak.com/s?url=${url}`, { method: 'POST' })
      .then(response => response.json())
      .then(data => {
        if (data.value) {
          const shortUrl = data.value.shortenedValue;
          resultDiv.innerHTML = `<a href="${shortUrl}" target="_blank">${shortUrl}</a><br>I've already copied it for you ❤`;
          copyToClipboard(shortUrl);
        } else {
          resultDiv.textContent = 'Error: Sorry m8 smth went wrong.';
        }
      })
      .catch(error => {
        console.error('Error:', error);
        if (error.status === 400) {
          resultDiv.textContent = 'Error: Smth is wrong with the URL.';
        } else {
          resultDiv.textContent = 'Error: Oops, looks like I f*ckd smth, no worries I will fix that.';
        }
      });
  }

  return (
      <div className="area" >
        <ul className="circles">
            {Array.from({length: 15}).map(i => (
              <li key={i}></li>
            ))}
        </ul>
      <img src='/ltly.svg' alt="Logo" className="logo" />
      <form onSubmit={e => shortenUrl(e)} className="url-form">
        <input
          placeholder='Long messy URL'
          value={url}
          onChange={e => handleUrlChange(e.target.value)}
          className={`url-input ${valid || url.length === 0 ? '' : 'invalid'}`}
        />
        <button type='submit' className="submit-button" disabled={!valid}>
          {valid ? 'Make this *** short!' : `That's not an URL`}
        </button>
      </form>
      <div id={'result'} className="result-div">
      </div>
    </div>
  );
}

export default App;
