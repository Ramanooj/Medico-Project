import React, { useEffect} from 'react';
import { Link } from 'react-router-dom';
import AOS from 'aos';
import 'aos/dist/aos.css';
import bgImage from '../../images/bg-image.jpg';

export default function NavbarSection()
{
    useEffect(() => { 
        AOS.init({ duration: 1000});
    }, []);
  
    return (
      <div className='h-screen'>  
        <img src={bgImage} alt="Hero-section Image" className='w-full h-full object-cover'/>
        
        <div className="absolute bottom-16 left-10 text-white">
            <p className="font-montserrat text-xl font-bold mb-2">Family Healthcare System in Canada</p>
            <h1 className="text-9xl font-lora font-bold mt-2 leading-tight">
              Healthcare <br />
              one click away
            </h1>
            <button className="bg-yellow-200 text-blue-900 font-semibold py-3 px-6 rounded-full mt-6
           hover:bg-yellow-300">
              <Link to={'/login'}>Book an Appointment</Link>
            </button>
        </div>
    </div>
  );
}
