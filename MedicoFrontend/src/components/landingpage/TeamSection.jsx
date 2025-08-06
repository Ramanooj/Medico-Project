// eslint-disable-next-line no-unused-vars
import React, { useEffect } from 'react';
import AOS from 'aos';
import 'aos/dist/aos.css';
import img2A from '../../images/img2-a.jpg';
import img2B from '../../images/img2-b.jpg';
import img2C from '../../images/img2-c.jpg';
import img2D from '../../images/img2-d.jpg';

export default function TeamSection() {
  useEffect(() => {
    AOS.init({ duration: 1000 });
  }, []);

  return (
    <div className="font-sans px-8 py-16 bg-[#DDE9FF]">
      <div className="max-w-7xl mx-auto space-y-16">

        {/* Our Team Section */}
        <div data-aos="fade-right" className="space-y-6">
          <button className="bg-gray-800 text-white py-2 px-4 rounded-full">
            Our team
          </button>
          <h2 className="text-6xl font-normal font-lora">The faces behind our <br /> exceptional care</h2>
          <p className="text-gray-600 text-lg font-montserrat">
            This is the space to introduce the Services section. Briefly describe the types of <br/>
            services offered and highlight any special benefits or features.
          </p>
        </div>

        <div data-aos="fade-up" className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8">
          {/* Team Member 1 */}
          <div className="text-center space-y-4">
            <div className="w-full rounded-5xl overflow-hidden shadow-lg">
              <img
                src={img2A}
                alt="Dr. John Wang"
                className="w-full h-auto object-cover"
              />
            </div>
            <div>
              <p className="text-xl font-semibold">Dr. John Wang</p>
              <p className="text-gray-500">Family Doctor</p>
            </div>
          </div>
          {/* Team Member 2 */}
          <div className="text-center space-y-4">
            <div className="w-full rounded-3xl overflow-hidden shadow-lg">
              <img
                src={img2B}
                alt="Dr. Talya Sulami"
                className="w-full h-auto object-cover"
              />
            </div>
            <div>
              <p className="text-xl font-semibold">Dr. Talya Sulami</p>
              <p className="text-gray-500">Pediatric</p>
            </div>
          </div>
          {/* Team Member 3 */}
          <div className="text-center space-y-4">
            <div className="w-full rounded-5xl overflow-hidden shadow-lg">
              <img
                src={img2C}
                alt="Dina Pierce"
                className="w-full h-auto object-cover"
              />
            </div>
            <div>
              <p className="text-xl font-semibold">Dina Pierce</p>
              <p className="text-gray-500">Family Doctor</p>
            </div>
          </div>
          {/* Team Member 4 */}
          <div className="text-center space-y-4">
            <div className="w-full rounded-3xl overflow-hidden shadow-lg">
              <img
                src={img2D}
                alt="Dr. Martin Sunak"
                className="w-full h-auto object-cover"
              />
            </div>
            <div>
              <p className="text-xl font-semibold">Dr. Martin Sunak</p>
              <p className="text-gray-500">Family Doctor</p>
            </div>
          </div>
        </div>

        {/* Testimonials Section */}
        <div data-aos="fade-left" className="space-y-6 mt-16">
          <button className="bg-gray-800 text-white py-2 px-4 rounded-full">
            Reviews
          </button>
          <h2 className="text-6xl font-normal font-lora">What our patients <br/> say about us</h2>
        </div>

        <div data-aos="fade-up" className="grid grid-cols-1 md:grid-cols-3 gap-8 mt-10">
          {/* Testimonial 1 */}
          <div className="bg-white rounded-3xl p-8 shadow-md">
            <blockquote className="text-gray-600 italic text-lg mb-4">
            <q>Booking an appointment has never been easier! The online syste is user-friendly, and I felt cared for every step of the way..</q>
            </blockquote>
            <p className="text-gray-900 font-bold">Shimit D.</p>
          </div>
          {/* Testimonial 2 */}
          <div className="bg-white rounded-3xl p-8 shadow-md">
            <blockquote className="text-gray-600 italic text-lg mb-4">
            <q>From the online booking to my consultation, everything was seamless and professional.Highly recommend!</q>
            </blockquote>
            <p className="text-gray-900 font-bold">Ella H.</p>
          </div>
          {/* Testimonial 3 */}
          <div className="bg-white rounded-3xl p-8 shadow-md">
            <blockquote className="text-gray-600 italic text-lg mb-4">
              <q>I finally found a healthcare team that genuinely cares. The experience was amazing, and I didn&#39;t have to wait weeks to get an appointment.</q>
            </blockquote>
            <p className="text-gray-900 font-bold">Amanda B.</p>
          </div>
        </div>
      </div>
    </div>
  );
}