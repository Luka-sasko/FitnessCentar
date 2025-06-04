import React from 'react';
import { NavLink } from 'react-router-dom';
import '../../styles/Navbar.css'; 

const Navbar = () => {
  const links = [
    { name: 'Home', path: '/' },
    { name: 'Discounts', path: '/discounts' },
    { name: 'Exercises', path: '/exercises' },
    { name: 'Food', path: '/foods' },
    { name: 'Meals', path: '/meals' },
    { name: 'Meal Plans', path: '/mealplans' },
    { name: 'Meal Plan Meals', path: '/mealplanmeals' },
    { name: 'Subscriptions', path: '/subscriptions' },
    { name: 'Users', path: '/profile' },
    { name: 'Workout Plans', path: '/workoutplans' },
    { name: 'Workout Plan Exercises', path: '/workoutplanexercises' }
  ];

  return (
    <nav className="navbar">
      <ul>
        {links.map(link => (
          <li key={link.path}>
            <NavLink
              to={link.path}
              className={({ isActive }) => isActive ? 'active' : ''}
            >
              {link.name}
            </NavLink>
          </li>
        ))}
      </ul>
    </nav>
  );
};

export default Navbar;
