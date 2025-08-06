import React, { useState } from "react";
import {
  LocalUser,
  RemoteUser,
  useJoin,
  useLocalMicrophoneTrack,
  useLocalCameraTrack,
  usePublish,
  useRemoteUsers,
  useIsConnected,
  useRTCClient
} from "agora-rtc-react";
import { FaMicrophone, FaMicrophoneSlash, FaVideo, FaVideoSlash, FaPhoneSlash } from "react-icons/fa";

function VideoCallPage({ channelName, appId, token, setCalling }) {
  const [micOn, setMic] = useState(true);
  const [cameraOn, setCamera] = useState(true);

  useJoin({
    useRTCClient,
    appid: appId,
    channel: channelName,
    token: token || null
  });

  const { localMicrophoneTrack } = useLocalMicrophoneTrack(micOn);
  const { localCameraTrack } = useLocalCameraTrack(cameraOn);

  usePublish([localMicrophoneTrack, localCameraTrack]);

  const remoteUsers = useRemoteUsers();
  const isConnected = useIsConnected();

  return (
    <div className="flex items-center justify-center bg-gray-100">
      <div className="w-full h-full max-w-4xl bg-white rounded-lg shadow-md p-6">
        {isConnected ? (
          <div>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-6 h-80">
              <div className="user bg-gray-200 rounded-lg shadow-sm overflow-hidden relative flex items-center justify-center">
                <LocalUser
                  audioTrack={localMicrophoneTrack}
                  cameraOn={cameraOn}
                  micOn={micOn}
                  videoTrack={localCameraTrack}
                  cover="https://www.agora.io/en/wp-content/uploads/2022/10/3d-spatial-audio-icon.svg"
                />
              <samp className="user-name text-white bg-black bg-opacity-50 p-1 rounded-md absolute bottom-1 left-1">
                You
              </samp>
              </div>

              {remoteUsers.map((user) => (
                <div
                  className="user bg-gray-200 rounded-lg shadow-sm overflow-hidden relative flex items-center justify-center"
                  key={user.uid}
                >
                  <RemoteUser
                    cover="https://www.agora.io/en/wp-content/uploads/2022/10/3d-spatial-audio-icon.svg"
                    user={user}
                  />
                </div>
              ))}
            </div>


            <div className="flex justify-center">
              <div className="flex space-x-4">
                <button
                  className="btn bg-gray-800 text-white p-3 rounded-full hover:bg-blue-600 focus:outline-none"
                  onClick={() => setMic((prev) => !prev)}>
                  
                  {micOn ? <FaMicrophone /> : <FaMicrophoneSlash className="text-red-500" />}
                </button>


                <button
                  className="btn bg-red-500 text-white p-3 rounded-full hover:bg-red-600 focus:outline-none"
                  onClick={() => setCalling(false)}>

                  <FaPhoneSlash />
                </button>

                <button
                  className="btn bg-gray-800 text-white p-3 rounded-full hover:bg-blue-600 focus:outline-none"
                  onClick={() => setCamera((prev) => !prev)}>

                  {cameraOn ? <FaVideo /> : <FaVideoSlash className="text-red-500" />}

                </button>


              </div>
            </div>

          </div>
        ) : (
          <div className="flex justify-center items-center h-64">
            <p className="text-gray-500">Connecting...</p>
          </div>
        )}
      </div>
    </div>
  );
}

export default VideoCallPage;
