import React, { useState, DragEvent, ChangeEvent } from 'react';
import './App.css';

const App: React.FC = () => {
  const [file, setFile] = useState<File | null>(null);
  const [resultImage, setResultImage] = useState<string | null>(null);
  
  
  const handleDrop = (e: DragEvent<HTMLDivElement>) => {
    e.preventDefault();
    const droppedFile = e.dataTransfer.files[0];
    setFile(droppedFile);
  };

  const handleFileChange = (e: ChangeEvent<HTMLInputElement>) => {
    if (e.target.files) {
      setFile(e.target.files[0]);
    }
  };
  
  
  return (
    <div className="App">
      <header className="App-header">
        <div 
          className="drop-zone" 
          onDrop={handleDrop} 
        >
          <h1>rmbg</h1>
          <input 
            type="file" 
            onChange={handleFileChange} 
            style={{ display: 'none' }} 
            id="fileInput"
          />
          <button onClick={() => document.getElementById('fileInput')?.click()}>
            Select Image
          </button>
        </div>
      </header>
    </div>
    );
}

export default App;
