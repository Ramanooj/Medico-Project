// eslint-disable-next-line no-unused-vars
import React, { useEffect } from 'react';
import AOS from 'aos';
import 'aos/dist/aos.css';
import img1 from '../../images/image1.jpeg';
import img4A from '../../images/img4-a.jpg';

export default function AboutSection() {
    useEffect(() => {
        AOS.init({ duration: 1000});
    }, []);

    return (

  <div className="font-sans px-8 py-16">
  {/* About Us Section */}
  <div className="max-w-7xl mx-auto grid grid-cols-1 md:grid-cols-2 gap-8 items-center">
    {/* Text Section */}
    <div data-aos="fade-right" className="space-y-6">
      <button className="bg-gray-800 text-white py-2 px-4 rounded-full">About Us</button>
      <h2 className="text-9xl font-lora font-normal">Your Wellness, Our Mission</h2>
      <p className="text-gray-600 font-montserrat text-lg">
      Our healthcare system is designed with you in mind. We bridge the gap between quality healthcare and the convenience of modern technology, ensuring everyone can access essential medical services without the hassle.
      </p>
    </div>

    {/* Image Section */}
    <div className="flex flex-col gap-8">
      <div data-aos="zoom-in" className="overflow-hidden rounded-3xl">
        <img
          src={img1} // Replace with actual image URL
          alt="Clinic Reception"
          className="w-full h-auto object-cover"
        />
      </div>
      <div data-aos="zoom-in-up" className="overflow-hidden rounded-3xl">
        <img
          src={img4A} // Replace with actual image URL
          alt="Doctor with Child"
          className="w-full h-auto object-cover"
        />
      </div>
    </div>
  </div>
</div>
    );
}