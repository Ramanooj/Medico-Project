import React from 'react';
import medico from '../../images/medico.jpeg';

export default function Footer() {
  return (
    <footer className="bg-gray-900 text-white py-16 px-8">
      <div className="max-w-7xl mx-auto grid grid-cols-1 md:grid-cols-5 gap-8">
        
        {/* Logo Section */}
        <div className="md:col-span-1">
        <a href="/">
                    <img src={medico} className='h-16 rounded-4xl' alt="Medico-Logo" />
            </a>
          <h3 className="text-2xl font-bold mb-4">Medico</h3>
        </div>

        {/* Contact Information */}
        <div className="md:col-span-1 space-y-4">
          <h4 className="font-semibold">Get in Touch</h4>
          <p>123-456-7890</p>
          <p>hello@medico.com</p>
          <h4 className="font-semibold mt-4">Visit us</h4>
          <p>7899 McLaughlin Rd</p>
          <p>Bram/</p>
        </div>
      </div>

{/* Bottom Copyright Section */}
<div className="max-w-7xl mx-auto mt-12 border-t border-gray-700 pt-6 text-sm text-gray-400">
  <p>© 2024 by Medico. </p>
</div>
</footer>
);
}
