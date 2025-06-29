import React from "react";
import { Link } from "react-router-dom";
import "../styles/landing_page.css";

const LandingPage = () => {
    return (
        <div className="landing-page">
            <header className="landing-header">
                <h1>Welcome to Fitness App</h1>
                <p>Your personal assistant for health, nutrition, and workouts.</p>
                <div className="landing-buttons">
                    <Link to="/login" className="btn">Login</Link>
                    <Link to="/register" className="btn">Register</Link>
                </div>
            </header>

            <main className="landing-content">

                <section className="features-section">
                    <h2>App Features</h2>
                    <ul>
                        <li>✔ View and track meal plans</li>
                        <li>✔ Create and manage workout plans</li>
                        <li>✔ Log your weight and height</li>
                        <li>✔ Connect with a personal coach</li>
                    </ul>
                </section>
                <section className="gallery-section">
                    <h2>Explore the App</h2>
                    <div className="image-row">
                        <figure>
                            <figcaption>Workout Plans</figcaption>
                            <img src="/images/workout.png" alt="Workout plan" />
                        </figure>
                        <figure>
                            <figcaption>Meal Plans</figcaption>
                            <img src="/images/meal.png" alt="Meal plan" />
                        </figure>
                        <figure>
                            <figcaption>Personal Coaching</figcaption>
                            <img src="/images/coach.png" alt="Personal coach" />
                        </figure>
                    </div>
                </section>


            </main>

            <footer className="landing-footer">
                <p>© {new Date().getFullYear()} Fitness App | All rights reserved</p>
            </footer>
        </div>
    );
};

export default LandingPage;
