import React, { useEffect, useState } from 'react';
import AOS from 'aos';
import { Link } from 'react-router-dom';
import { FiMenu, FiX, FiMessageCircle, FiTrash2, FiArrowUp } from 'react-icons/fi';
import 'aos/dist/aos.css';
import medico from '../../images/medico.jpeg';

export default function Navbar() {
    useEffect(() => {
        AOS.init({ duration: 1000 });
    }, []);

    const [isMenuOpen, setIsMenuOpen] = useState(false);
    const [isChatOpen, setIsChatOpen] = useState(false);
    const [messages, setMessages] = useState([]);
    const [input, setInput] = useState('');

    const handleMenuItemClick = () => {
        setIsMenuOpen(false);
    };
    const handleChatbotClick = () => {
        setIsChatOpen(!isChatOpen);
    };


    const handleSendMessage = async () => {
        if (!input.trim()) return;
    
        setMessages((prevMessages) => [...prevMessages, { sender: 'user', text: input }]);
    
        try {
            const response = await fetch('https://projectmedico.site/api/Chat', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ Prompt: input }),
            });
    
            const data = await response.json();
            setMessages((prevMessages) => [
                ...prevMessages,
                { sender: 'bot', text: data.reply },
            ]);
        } 
        catch (error) {
            console.error('Error with chatbot request:', error);
            setMessages((prevMessages) => [
                ...prevMessages,
                { sender: 'bot', text: "An error occurred while trying to reach the chatbot." },
            ]);
        }
    
        setInput('');
    };

    const handleClearChat = () => {
        setMessages([]);
    };

    const handleKeyDown = (event) => {
        if (event.key === 'Enter') {
            handleSendMessage();
        }
    };

    const handleScrollToTop = () => {
        window.scrollTo({ top: 0, behavior: 'smooth' });
    };



    return (
        <div className='py-1'>
            <header className='w-full h-20 flex items-center  dark:bg-gray-800 rounded-xl px-4'>
                <a href="/">
                    <img src={medico} className='h-16 rounded-3xl' alt="Medico-Logo" />
                </a>
                
                <p className='text-black dark:text-white font-montserrat font-bold text-4xl ml-4'>Medico</p>
                <nav className="ml-auto flex items-center">
                    <button
                        type="button"
                        className="rounded-full p-2 bg-white text-lg font-normal text-black hover:text-blue-700 
                        focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 mr-4"
                        id="chatbot-button"
                        onClick={handleChatbotClick}

                    >
                        <FiMessageCircle size={24} />
                    </button>
                    <button
                        type="button"
                        className="rounded-full p-2 bg-white text-lg font-normal text-black hover:text-blue-700 
                        focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
                        id="menu-button"
                        aria-expanded={isMenuOpen}
                        aria-haspopup="true"
                        onClick={() => setIsMenuOpen(!isMenuOpen)}
                    >
                        {isMenuOpen ? <FiX size={24} /> : <FiMenu size={24} />}
                    </button>
                    {isMenuOpen && (
                        <div
                            id="menu-items"
                            className="origin-top-right absolute right-0 mt-40 w-48 rounded-md 
                            shadow-lg bg-white dark:bg-gray-700 ring-1 ring-black ring-opacity-5 focus:outline-none z-50"
                            role="menu"
                            aria-orientation="vertical"
                            aria-labelledby="menu-button"
                        >
                            <div className="py-1">
                                {['Login', 'Register'].map((item, index) => (
                                    <Link
                                        key={index}
                                        to={item == 'Register' ? '/signup' : `/${item.toLowerCase().replace(/ /g, '-')}`}
                                        className='block px-4 py-2 text-lg text-black dark:text-white font-lora
                                         hover:text-blue-700 dark:hover:text-blue-400'
                                        role='menuitem'
                                        onClick={handleMenuItemClick}
                                    >
                                        {item}
                                    </Link>
                                ))}
                            </div>
                        </div>
                    )}
                </nav>
            </header>

            {/* Chat window */}
            {isChatOpen && (
                <div className="fixed bottom-4 right-4 w-80 bg-white dark:bg-gray-800 rounded-lg shadow-lg p-4 z-50">
                    {/* Header with Close and Clear buttons */}
                    <div className="flex justify-between items-center mb-2">
                        <h3 className="text-lg font-semibold text-black dark:text-white">Chat with ChatGPT</h3>
                        <div className="flex items-center space-x-2">
                            <button
                                onClick={handleClearChat}
                                className="text-gray-500 hover:text-gray-700 dark:text-gray-300 dark:hover:text-white"
                            >
                                <FiTrash2 size={20} />
                            </button>
                            <button
                                onClick={() => setIsChatOpen(false)}
                                className="text-gray-500 hover:text-gray-700 dark:text-gray-300 dark:hover:text-white"
                            >
                                <FiX size={20} />
                            </button>
                        </div>
                    </div>
                    <div className="max-h-64 h-64 overflow-y-auto mb-4">
                        {messages.map((msg, index) => (
                            <div key={index} className={`mb-2 ${msg.sender === 'bot' ? 'text-left' : 'text-right'}`}>
                                <div
                                    className={`inline-block p-2 rounded-lg ${
                                        msg.sender === 'bot' ? 'bg-blue-500 text-white' : 'bg-green-500 text-white'
                                    }`}
                                >
                                    {msg.text}
                                </div>
                            </div>
                        ))}
                    </div>
                    <div className="flex items-center space-x-2">
                        <input
                            type="text"
                            value={input}
                            onChange={(e) => setInput(e.target.value)}
                            onKeyDown={handleKeyDown}
                            placeholder="Type your message..."
                            className="flex-grow border rounded-lg p-2"
                        />
                        <button
                            onClick={handleSendMessage}
                            className="bg-blue-500 text-white px-4 py-2 rounded-lg hover:bg-blue-600"
                        >
                            Send
                        </button>
                    </div>
                </div>
            )}

            <button
                onClick={handleScrollToTop}
                className={`fixed ${isChatOpen ? 'bottom-85' : 'bottom-8'} right-8 bg-gray-300 text-black p-3 rounded-full shadow-lg
                 hover:bg-gray-400 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500 z-50`}
            >
                <FiArrowUp size={24} />
            </button>
        </div>
    );
}

