import React from "react";
import { Link } from "react-router-dom";
import "../styles/landing_page.css";

const LandingPage = () => {
    return (
        <div className="landing-page">
            <header className="landing-header">
                <h1>WELCOME TO FITNESS APP</h1>
                <p>Your personal assistant for HEALTH, NUTRITION, and WORKOUTS.</p>
                <div className="landing-buttons">
                    <Link to="/login" className="btn">LOGIN</Link>
                    <Link to="/register" className="btn">REGISTER</Link>
                </div>
            </header>

            <main style={{ paddingTop: 100 }} className="landing-content">
                <section className="features-section">
                    <ul>
                        <li>✔ VIEW AND TRACK MEAL PLANS</li>
                        <li>✔ CREATE AND MANAGE WORKOUT PLANS</li>
                        <li>✔ LOG YOUR WEIGHT AND HEIGHT</li>
                        <li>✔ CONNECT WITH PERSONAL COACH</li>
                    </ul>
                </section>
                <section className="gallery-section">
                    <h2>EXPLORE THE APP </h2>
                    <div className="image-row">
                        <figure>
                            <figcaption>WORKOUT PLANS</figcaption>
                            <img src="/images/workout.png" alt="Workout plan" />
                        </figure>
                        <figure>
                            <figcaption>MEAL PLANS</figcaption>
                            <img src="/images/meal.png" alt="Meal plan" />
                        </figure>
                        <figure>
                            <figcaption>PERSONAL COACHING</figcaption>
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
