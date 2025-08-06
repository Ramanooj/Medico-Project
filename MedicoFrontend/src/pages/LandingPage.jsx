import React, { useEffect } from 'react';
import AOS from 'aos';
import 'aos/dist/aos.css';
import HeroSection from "../components/landingpage/HeroSection";
import AboutSection from "../components/landingpage/AboutSection";
import PatientCareSection from "../components/landingpage/PatientCareSection";
import TeamSection from "../components/landingpage/TeamSection";
import ContactUsSection from "../components/landingpage/ContactUsSection";
import Navbar from '../components/reusable/Navbar';

export default function LandingPage() 
{
    useEffect(() => {
        AOS.init({ duration: 1000});
    }, []);

    return(
        <>
            <Navbar /> 
            <HeroSection /> 
            <AboutSection />
            <PatientCareSection /> 
            <TeamSection /> 
            <ContactUsSection /> 
        
        </>
    );
}