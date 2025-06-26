import React from 'react';
import { NavLink } from 'react-router-dom';
import logo from '../../assets/icons/barbell.png';
import '../../styles/Navbar.css';

const links = [
  { name: 'Home', path: '/' },
  { name: 'Discounts', path: '/discounts' },
  { name: 'Meal Plans', path: '/mealplans' },
  { name: 'Subscriptions', path: '/subscriptions' },
  { name: 'Workout Plans', path: '/workoutplans' },
  { name: 'Exercises', path: '/exercises' },
];

const Navbar = () => (
  <nav className="navbar">
    <img
      src={logo}
      alt="App logo"
      className="navbar-logo"
    />
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

export default Navbar;
