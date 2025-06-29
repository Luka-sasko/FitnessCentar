import React, { useEffect, useState } from "react";
import { observer } from "mobx-react-lite";
import { exerciseStore } from "../stores/ExerciseStore";
import ConfirmDeleteModal from "../components/confirmDeleteModal/ConfirmDeleteModal";
import AddExerciseModal from "../components/exercise/AddExerciseModal";
import GenericTable from "../components/Common/GenericTable";
import "../App.css";

const ExercisePage = observer(() => {
  const [addModalOpen, setAddModalOpen] = useState(false);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [deleteId, setDeleteId] = useState(null);
  const [editData, setEditData] = useState(null);

  useEffect(() => {
    exerciseStore.fetchAll();
  }, []);

  const handleAdd = () => {
    setEditData(null);
    setAddModalOpen(true);
  };

  const handleEdit = (exercise) => {
    setEditData(exercise);
    setAddModalOpen(true);
  };

  const openDeleteModal = (id) => {
    setDeleteId(id);
    setDeleteModalOpen(true);
  };

  const handleDeleteConfirmed = async () => {
    await exerciseStore.deleteExercise(deleteId);
    setDeleteModalOpen(false);
    setDeleteId(null);
  };

  const addButton = (
    <button
      style={{
        background: "black",
        color: "white",
        border: "none",
        borderRadius: "6px",
        padding: "8px 20px",
        fontSize: "1em",
        fontWeight: 600,
        cursor: "pointer",
        boxShadow: "0 2px 8px #4f8cff22",
        transition: "background 0.18s, box-shadow 0.18s",
      }}
      onClick={handleAdd}
    >
      ➕ ADD EXERCISE
    </button>
  );

  return (
    <div style={{ padding: 24, maxWidth: 900, margin: "0 auto" }}>
      <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: 18 }}>
        <h2>All Exercises</h2>
        {addButton}
      </div>

      <GenericTable
        store={exerciseStore}
        onRowClick={handleEdit}
        onDeleteRow={openDeleteModal}
      />

      <AddExerciseModal
        open={addModalOpen}
        onClose={() => setAddModalOpen(false)}
        initialData={editData}
        onSubmit={async (data) => {
          if (editData) {
            await exerciseStore.updateExercise(editData.Id, data);
          } else {
            await exerciseStore.createExercise(data);
          }
          setAddModalOpen(false);
          setEditData(null);
        }}
      />  


      <ConfirmDeleteModal
        open={deleteModalOpen}
        onClose={() => setDeleteModalOpen(false)}
        onConfirm={handleDeleteConfirmed}
        text="Are you sure you want to delete this exercise from the database? This cannot be undone."
      />
    </div>
  );
});

export default ExercisePage;
