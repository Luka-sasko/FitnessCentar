import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import HomePage from './HomePage.js';
import DiscountPage from './DiscountPage.js';
import ExercisePage from './ExercisePage.js';
import FoodPage from './FoodPage.js';
import MealPage from './MealPage.js';
import MealPlanPage from './MealPlanPage.js';
import MealPlanMealPage from './MealPlanMealPage.js';
import SubscriptionPage from './SubscriptionPage.js';
import UserProfilePage from './UserPage/ProfilePage.js';
import LoginPage from './UserPage/LoginPage.js';
import RegisterPage from './UserPage/RegisterPage.js';
import WorkoutPlanPage from './WorkoutPlanPage.js';
import WorkoutPlanExercisePage from './WorkouotPlanExercisePage.js';
import Navbar from '../components/Common/Navbar.js';

const IndexRoutePage = () => {
    return (
        <Router>
            <Navbar />
            <Routes>
                <Route path="/" element={<HomePage />} />
                <Route path="/discounts" element={<DiscountPage />} />
                <Route path="/exercises" element={<ExercisePage />} />
                <Route path="/foods" element={<FoodPage />} />
                <Route path="/food" element={<FoodPage />} /> { }
                <Route path="/meals" element={<MealPage />} />
                <Route path="/mealplans" element={<MealPlanPage />} />
                <Route path="/mealplanmeals" element={<MealPlanMealPage />} />
                <Route path="/subscriptions" element={<SubscriptionPage />} />
                <Route path="/profile" element={<UserProfilePage />} />
                <Route path="/login" element={<LoginPage />} />
                <Route path="/register" element={<RegisterPage />} />
                <Route path="/workoutplans" element={<WorkoutPlanPage />} />
                <Route path="/workoutplanexercises" element={<WorkoutPlanExercisePage />} />
            </Routes>
        </Router>
    );
};

export default IndexRoutePage;
