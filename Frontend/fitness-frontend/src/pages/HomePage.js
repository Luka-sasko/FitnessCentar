import React from 'react';
import { Link } from 'react-router-dom';
//import '../styles/HomePage.css';


const HomePage = () => {

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