import React, { useState, DragEvent } from 'react';
import './App.css';

const App: React.FC = () => {
  const [file, setFile] = useState<File | null>(null);
  const handleDrop = (e: DragEvent<HTMLDivElement>) => {
    e.preventDefault();
    const droppedFile = e.dataTransfer.files[0];
    setFile(droppedFile);
  };
  
  
  return (
    <div className="App">
      <header className="App-header">
        <div 
          className="drop-zone" 
          onDrop={handleDrop} 
        >
          <button onClick={() => document.getElementById('fileInput')?.click()}>
            Select Image
          </button>
        </div>
      </header>
    </div>
    );
}

export default App;
