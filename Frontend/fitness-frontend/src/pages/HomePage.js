import { Link } from 'react-router-dom';
import React, { useEffect } from "react";
import axios from "axios";
//import '../styles/HomePage.css';


const HomePage = () => {
    useEffect(() => {
  const autoLogin = async () => {
    const existing = JSON.parse(localStorage.getItem("user") || "{}");
    if (existing?.token) return;

    try {
      const response = await axios.post("https://localhost:44366/Login", new URLSearchParams({
        grant_type: "password",
        username: "johndoe@example.com'",
        password: "hash_password"
      }), {
        headers: { "Content-Type": "application/x-www-form-urlencoded" }
      });

      const { access_token, token_type, expires_in } = response.data;

      localStorage.setItem("user", JSON.stringify({
        token: access_token,
        tokenType: token_type,
        expiresIn: expires_in
      }));

      console.log("✅ Auto login successful!");
    } catch (err) {
      console.error("❌ Auto login failed:", err);
    }
  };

  autoLogin();
}, []);


    const pages = [
        { name: 'Login', path: '/login' },
        { name: 'Register', path: '/register' },
        { name: 'Profile', path: '/profile' },
        { name: 'Discounts', path: '/discounts' },
        { name: 'Exercises', path: '/exercises' },
        { name: 'Food', path: '/foods' },
        { name: 'Meals', path: '/meals' },
        { name: 'Meal Plans', path: '/mealplans' },
        { name: 'Meal Plan Meals', path: '/mealplanmeals' },
        { name: 'Subscriptions', path: '/subscriptions' },
        { name: 'Workout Plans', path: '/workoutplans' },
        { name: 'Workout Plan Exercises', path: '/workoutplanexercises' }
    ];

    return (
        <div style={{ padding: '20px' }}>
            <h1>🏠 Fitness Centar - Home</h1>
            <p>Dobrodošli! Odaberite neku od stranica:</p>
            <ul>
                {pages.map((page) => (
                    <li key={page.path}>
                        <Link to={page.path}>{page.name}</Link>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default HomePage;