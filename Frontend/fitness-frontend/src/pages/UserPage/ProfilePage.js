import React, { useState, useEffect } from "react";
import { observer } from "mobx-react-lite";
import { userStore } from "../../stores/UserStore";
import EditProfileModal from "../../components/User/EditProfileModal.js";
import ResetPasswordModal from "../../components/User/ResetPasswordModal";

import "../../styles/auth.css";

const ProfilePage = observer(() => {
  const [user, setUser] = useState(null);
  const [showModal, setShowModal] = useState(false);
  const [showPasswordModal, setShowPasswordModal] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await userStore.fetch();
        setUser(response.data);
      } catch {
        setError("❌ Failed to fetch user profile.");
      }
    };

    fetchData();
  }, []);

  if (!user) return <div className="profile-page">LOADING...</div>;

  return (
    <div className="profile-wrapper">
      <h2 className="profile-title">YOUR PROFILE</h2>

      <table className="profile-table">
        <tbody>
          <tr>
            <th>ROLE:</th>
            <td>{user.Role}</td>
          </tr>
          <tr>
            <th>FIRSTNAME:</th>
            <td>{user.Firstname}</td>
          </tr>
          <tr>
            <th>LASTNAME:</th>
            <td>{user.Lastname}</td>
          </tr>
          <tr>
            <th>CONTACT:</th>
            <td>{user.Contact}</td>
          </tr>
          <tr>
            <th>BIRTHDATE:</th>
            <td>{user.Birthdate?.split("T")[0]}</td>
          </tr>
          <tr>
            <th>WEIGHT:</th>
            <td>{user.Weight} kg</td>
          </tr>
          <tr>
            <th>HEIGHT:</th>
            <td>{user.Height} cm</td>
          </tr>
        </tbody>
      </table>

      <div className="edit-btn-wrapper">
        <button onClick={() => setShowModal(true)} className="edit-btn">
          EDIT PROFILE
        </button>

        <button onClick={() => setShowPasswordModal(true)} className="edit-btn reset-btn">
          RESET PASSWORD
        </button>
      </div>


      {showModal && (
        <EditProfileModal
          user={user}
          onClose={() => setShowModal(false)}
          onUpdate={async () => {
            const res = await userStore.fetch();
            setUser(res.data);
          }}
        />
      )}

      {showPasswordModal && (
        <ResetPasswordModal
          onClose={() => setShowPasswordModal(false)}
          onSubmit={async (form) => {
            await userStore.updatePassword({
              PasswordOld: form.passwordOld,
              PasswordNew: form.passwordNew
            });
          }}
        />
      )}


    </div>
  );

});

export default ProfilePage;
