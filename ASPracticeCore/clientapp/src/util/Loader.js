import React from 'react';
import {motion} from 'framer-motion';

const Loader = () => {

    const spinner = {
        margin: "0 auto",
        border: "1rem solid #f3f3f3",
        borderRadius: "50%",
        borderTop: "1rem solid #f50057",
        width: "5rem",
        height: "5rem",

    }
    const spinTransition = {
        loop:"Infinity",
        duration:1,
        ease:"linear"
    }

    return (<motion.div style={spinner} animate={{rotate:360}} transition={spinTransition}/>)

}

export default Loader