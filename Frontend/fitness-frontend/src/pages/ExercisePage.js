import React, { useEffect, useState } from "react";
import baseApi from "../api/BaseApi"; // prilagodi putanju prema svom projektu
import ConfirmDeleteModal from "../components/confirmDeleteModal/ConfirmDeleteModal";
import AddExerciseModal from "../components/exercise/AddExerciseModal"; // vidi ispod
import GenericTable from "../components/Common/GenericTable";

const ExercisePage = () => {
  const [exercises, setExercises] = useState([]);
  const [addModalOpen, setAddModalOpen] = useState(false);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [deleteId, setDeleteId] = useState(null);

  useEffect(() => {
    fetchExercises();
  }, []);

  const fetchExercises = async () => {
    try {
      const res = await baseApi.getAll("/exercise", { pageSize: 100 });
      setExercises(res.data.Items || []);
    } catch (e) {
      setExercises([]);
    }
  };

  const openDeleteModal = (id) => {
    setDeleteId(id);
    setDeleteModalOpen(true);
  };

  const handleDeleteConfirmed = async () => {
    await baseApi.delete(`/exercise?id=${deleteId}`);
    setDeleteModalOpen(false);
    setDeleteId(null);
    fetchExercises();
  };

  return (
    <div style={{ padding: 24, maxWidth: 900, margin: "0 auto" }}>
      <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: 18 }}>
        <h2>All Exercises</h2>
        <button
          style={{
            background: "#4f8cff",
            color: "#fff",
            border: "none",
            borderRadius: "6px",
            padding: "8px 20px",
            fontSize: "1em",
            fontWeight: 600,
            cursor: "pointer",
            boxShadow: "0 2px 8px #4f8cff22",
            transition: "background 0.18s, box-shadow 0.18s",
          }}
          onClick={() => setAddModalOpen(true)}
        >
          ➕ ADD EXERCISE
        </button>
      </div>

      <GenericTable
        items={exercises.map(ex => ({
          Id: ex.Id,
          Name: ex.Name,
          Desc: ex.Desc,
          Reps: ex.Reps,
          Sets: ex.Sets,
          RestPeriod: ex.RestPeriod
        }))}
        onDeleteRow={item => openDeleteModal(item.Id)}
      />

      <AddExerciseModal
        open={addModalOpen}
        onClose={() => setAddModalOpen(false)}
        onAdded={fetchExercises}
      />

      <ConfirmDeleteModal
        open={deleteModalOpen}
        onClose={() => setDeleteModalOpen(false)}
        onConfirm={handleDeleteConfirmed}
        text="Are you sure you want to delete this exercise from the database? This cannot be undone."
      />
    </div>
  );
};

export default ExercisePage;
