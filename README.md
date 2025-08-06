# Medico – Patient Scheduling & Telehealth Platform

A full-stack healthcare web application built as a capstone project by a team of 4 students. Medico allows patients to register, book appointments, receive automated reminders, and attend video consultations with doctors — all in a secure and scalable cloud-based environment.

---

## 🛠️ Tech Stack

**Frontend:** ReactJS, Tailwind CSS  
**Backend:** .NET Core Web API, C#, Entity Framework Core  
**Database:** MS SQL Server (Amazon RDS)  
**Cloud Services:**  
- Amazon Cognito – Authentication & role-based access  
- Amazon SNS – Appointment reminders  
- Amazon EventBridge – Event-driven workflows  
- AWS Elastic Beanstalk – Backend deployment  
- Amazon S3 + CloudFront – Frontend hosting  
- Google Distance Matrix API – Travel time calculation  
- Agora SDK – Video conferencing

---

## ✅ Features

- Secure user registration & login with Cognito (JWT-based)
- Role-based access control (Patient / Doctor)
- Book, view, and manage appointments
- SMS or email reminders via Amazon SNS
- Video calls between patients and doctors using Agora
- Travel time calculation to clinics using Google APIs
- Automated backend workflows (EventBridge)
- Fully deployed on AWS with CI/CD support

---

## 🚀 How to Run Locally

1. **Clone this repo**
```bash
git clone https://github.com/Ramanooj/Medico-Project.git
cd Medico-Project
```

2. **Frontend**
```bash
cd frontend
npm install
npm start
```

3. **Backend**
```bash
cd backend
# Ensure .NET SDK is installed
dotnet restore
dotnet run
```

4. **Environment Variables**
> Configure AWS credentials, Cognito, Agora, and DB connection strings in a `.env` file.

---

## 👨‍💻 Team

Built by 4 software development students at Sheridan College as part of our final capstone project.

---

## 📄 License

This project is for academic/demo purposes.
