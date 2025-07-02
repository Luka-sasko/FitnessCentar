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
    <button className="add-exercise-btn" onClick={handleAdd}>
      <svg width="20" height="20" className="add-plus-icon" viewBox="0 0 20 20" fill="none">
        <rect x="9" y="3" width="2" height="14" rx="1" fill="#222" />
        <rect x="3" y="9" width="14" height="2" rx="1" fill="#222" />
      </svg>
      ADD EXERCISE
    </button>
  );

  return (
    <div style={{ padding: 24, maxWidth: 900, margin: "0 auto" }}>
      <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: 18 }}>
        <h2 style={{ color: "white" }}>ALL EXERCISES</h2>
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
