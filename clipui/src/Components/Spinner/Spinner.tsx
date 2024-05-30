import React from 'react'
import './Spinner.css'

type Props = {}

const Spinner = (props: Props) => {
   return(
    <div className="spinner-overlay">
    <div className="spinner" />
    </div>
)
}

export default Spinner