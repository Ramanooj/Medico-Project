import React, { useState } from 'react';

const Chatbot = () => {
    const [messages, setMessages] = useState([]);
    const [input, setInput] = useState('');
    
    const sendMessage = async () => {
        if (!input.trim()) return;
        const newMessage = { sender: 'user', text: input };
        setMessages([...messages, newMessage]);
        try {
            const response = await fetch('https://projectmedico.site/api/Chat', {
                method: 'POST',
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ Prompt: input })
            });
            setMessages([...messages, newMessage, { sender: 'bot', text: response.data.reply }]);
        } 
        catch (error) {
            console.error('Error fetching bot reply:', error);
        }

        setInput('');
    };

    return (
        <div className="flex flex-col items-center justify-end h-screen p-4 bg-gray-100">
            <div className="w-full max-w-lg bg-white rounded-lg shadow-lg p-4 space-y-4">
                <div className="space-y-2 overflow-y-auto h-96">
                    {messages.map((msg, idx) => (
                        <div key={idx} className={`flex ${msg.sender === 'bot' ? 'justify-start' : 'justify-end'}`}>
                            <div
                                className={`p-2 rounded-lg text-white ${
                                    msg.sender === 'bot' ? 'bg-blue-500' : 'bg-green-500'
                                }`}
                            >
                                {msg.text}
                            </div>
                        </div>
                    ))}
                </div>
                <div className="flex space-x-2">
                    <input
                        type="text"
                        value={input}
                        onChange={(e) => setInput(e.target.value)}
                        placeholder="Type a message..."
                        className="flex-grow border rounded-lg p-2"
                    />
                    <button
                        onClick={sendMessage}
                        className="bg-blue-500 text-white rounded-lg px-4 py-2"
                    >
                        Send
                    </button>
                </div>
            </div>
        </div>
    );
};

export default Chatbot;
