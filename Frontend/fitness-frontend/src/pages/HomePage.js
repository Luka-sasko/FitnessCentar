import { Link } from 'react-router-dom';


const HomePage = () => {

  const pages = [
    
    { name: 'Profile', path: '/profile' },
    { name: 'Discounts', path: '/discounts' },
    { name: 'Subscriptions', path: '/subscriptions' },
    { name: 'Workout Plans', path: '/workoutplans' },
  ];

  return (
    <div style={{ padding: '20px', alignContent: 'center' }}>
      <h1 style={{ display: 'flex', alignItems: 'center', gap: '10px', fontSize: '2.2rem' }}>
        <span role="img" aria-label="dumbbell">🏋️‍♂️</span>
        <span style={{ fontWeight: 'bold', color: '#2c3e50' }}>FITNESS CENTAR</span>
      </h1>


      <div style={styles.grid}>
        {pages.map((page) => (
          <Link to={page.path} key={page.path} style={styles.card}>
            <h3>{page.name}</h3>
          </Link>
        ))}
      </div>
    </div>
  );
};

const styles = {
  grid: {
    display: 'grid',
    gridTemplateColumns: 'repeat(3, 1fr)',
    gap: '16px',
    marginTop: '20px'
  },
  card: {
    textDecoration: 'none',
    padding: '20px',
    border: '1px solid #ccc',
    borderRadius: '10px',
    backgroundColor: '#f9f9f9',
    color: '#333',
    textAlign: 'center',
    transition: 'transform 0.2s, box-shadow 0.2s',
    boxShadow: '0 2px 5px rgba(0,0,0,0.1)',
  },
};

export default HomePage;
