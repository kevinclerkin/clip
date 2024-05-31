import React, { useState, DragEvent, ChangeEvent } from 'react';
import './App.css';
import Spinner from './Components/Spinner/Spinner';

const App: React.FC = () => {
  const [file, setFile] = useState<File | null>(null);
  const [resultImage, setResultImage] = useState<string | null>(null);
  const [loading, setLoading] = useState<boolean>(false);
  
  
  
  const handleDrop = (e: DragEvent<HTMLDivElement>) => {
    e.preventDefault();
    const droppedFile = e.dataTransfer.files[0];
    setFile(droppedFile);
  };

  const handleDragOver = (e: DragEvent<HTMLDivElement>) => {
    e.preventDefault();
  };

  const handleFileChange = (e: ChangeEvent<HTMLInputElement>) => {
    if (e.target.files) {
      setFile(e.target.files[0]);
    }
  };

  const handleUpload = async () => {
    if (!file) return;
    
    const formData = new FormData();
    formData.append('file', file);

    setLoading(true);
   

    try {
      const response = await fetch('https://clipapi.azurewebsites.net/process-image', {
        method: 'POST',
        body: formData,
      });

      if (response.ok) {
        const imageBlob = await response.blob();
        const imageUrl = URL.createObjectURL(imageBlob);
        setResultImage(imageUrl);
      } else {
        console.error('Error processing image');
      }
    } catch (error) {
      console.error('Error uploading file:', error);
    } finally {
      setLoading(false);
    }
  };

  
  
  return (
    <div className="App">
      <header className="App-header">
      <div className="header-text">
          <div className="line line1">Remove the background</div>
          <div className="line line2">from any image</div>
          <div className="line line3">with Clip</div>
        </div>
        <div 
          className="drop-zone" 
          onDrop={handleDrop}
          onDragOver={handleDragOver}
        >
          <input 
            type="file" 
            onChange={handleFileChange} 
            style={{ display: 'none' }} 
            id="fileInput"
          />
          <button onClick={() => document.getElementById('fileInput')?.click()}>
            Select an Image
          </button>
          <p>or drag and drop an image here</p>
          <button onClick={handleUpload} disabled={!file}>
            Clip the Image
          </button>
        </div>
        {resultImage && (
          <div className="result-box">
            <h2>Download image</h2>
            <a href={resultImage} download="processed_image.png">
              <img src={resultImage} alt="Processed" />
            </a>
          </div>
        )}
        {loading && <Spinner />}
      </header>
    </div>
  );
}

export default App;
