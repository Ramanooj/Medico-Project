// eslint-disable-next-line no-unused-vars
import React, { useEffect } from 'react';
import AOS from 'aos';
import 'aos/dist/aos.css';
import img5 from '../../images/img5.jpg';

export default function ContactUsSection() {
  useEffect(() => {
    AOS.init({ duration: 1000 });
  }, []);

  return (
    <div className="font-sans px-8 py-16">
      <div className="max-w-7xl mx-auto grid grid-cols-1 md:grid-cols-2 gap-16 items-center">
        
        {/* Contact Form */}
        <div data-aos="fade-right">
          <h2 className="text-4xl font-normal font-lora mb-8">Contact us</h2>
          <form className="space-y-6">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div>
                <label htmlFor="first-name" className="block text-sm font-medium text-gray-700">
                  First name *
                </label>
                <input
                  type="text"
                  id="first-name"
                  required
                  className="mt-1 block w-full border-b-2 border-gray-400 focus:border-blue-500 outline-none p-2"
                />
              </div>
              <div>
                <label htmlFor="last-name" className="block text-sm font-medium text-gray-700">
                  Last name *
                </label>
                <input
                  type="text"
                  id="last-name"
                  required
                  className="mt-1 block w-full border-b-2 border-gray-400 focus:border-blue-500 outline-none p-2"
                />
              </div>
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div>
                <label htmlFor="email" className="block text-sm font-medium text-gray-700">
                  Email *
                </label>
                <input
                  type="email"
                  id="email"
                  required
                  className="mt-1 block w-full border-b-2 border-gray-400 focus:border-blue-500 outline-none p-2"
                />
              </div>
              <div>
                <label htmlFor="phone" className="block text-sm font-medium text-gray-700">
                  Phone
                </label>
                <input
                  type="tel"
                  id="phone"
                  className="mt-1 block w-full border-b-2 border-gray-400 focus:border-blue-500 outline-none p-2"
                />
              </div>
            </div>

            <div>
              <label htmlFor="message" className="block text-sm font-medium text-gray-700">
                Message
              </label>
              <textarea
                id="message"
                rows="4"
                className="mt-1 block w-full border-b-2 border-gray-400 focus:border-blue-500 outline-none p-2"
              ></textarea>
            </div>

            <button
              type="submit"
              className="w-full md:w-auto bg-blue-700 text-white py-3 px-6 rounded-3xl hover:bg-blue-800"
            >
              Submit
            </button>
          </form>
        </div>

        {/* Contact Image */}
        <div data-aos="fade-left" className="rounded-3xl overflow-hidden shadow-lg bg-[#FFF4C3] p-4">
          <img
            src={img5}// Replace with actual image URL
            alt="Contact Us"
            className="w-full h-auto object-cover rounded-2xl"
          />
        </div>
      </div>
    </div>
  );
}