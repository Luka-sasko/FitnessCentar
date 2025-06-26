import React, { useEffect, useState } from "react";
import { observer } from "mobx-react-lite";
import { workoutPlanStore } from "../stores/WorkOutPlanStore";
import GenericTable from "../components/Common/GenericTable";
import baseApi from "../api/BaseApi";
import ConfirmDeleteModal from "../components/confirmDeleteModal/ConfirmDeleteModal";
import AddWorkoutPlanModal from "../components/workoutPlan/AddWorkoutPlanModal";
import AddExerciseToPlanModal from "../components/exercise/AddExerciseToPlanModal";
import "../App.css";

const WorkoutPlanPage = observer(() => {
  const [selectedPlanExercises, setSelectedPlanExercises] = useState([]);
  const [currentWorkoutPlan, setCurrentWorkoutPlan] = useState(null);

  const [addModalOpen, setAddModalOpen] = useState(false);
  const [addExerciseModalOpen, setAddExerciseModalOpen] = useState(false);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [deleteTarget, setDeleteTarget] = useState({ type: null, id: null });

  useEffect(() => {
    workoutPlanStore.fetchAll();
  }, []);

  const openDeleteModal = (type, id) => {
    setDeleteTarget({ type, id });
    setDeleteModalOpen(true);
  };

  const handleDeleteConfirmed = async () => {
    setDeleteModalOpen(false);
    if (deleteTarget.type === "workoutplan") {
      await baseApi.delete(`/workoutplan?id=${deleteTarget.id}`);
      workoutPlanStore.fetchAll();
      setSelectedPlanExercises([]);
      setCurrentWorkoutPlan(null);
    } else if (deleteTarget.type === "exercise") {
      console.log(deleteTarget.id);
      await baseApi.delete(`/workoutplanexercise?id=${deleteTarget.id}`);
      if (currentWorkoutPlan) {
        await handleWorkoutPlanClick(currentWorkoutPlan);
      }
    }
    setDeleteTarget({ type: null, id: null });
  };

  const handleWorkoutPlanClick = async (workoutPlan) => {
    setCurrentWorkoutPlan(workoutPlan);
    try {
      const response = await baseApi.getAll("/workoutplanexercise", {
        workoutPlanId: workoutPlan.Id,
        pageNumber: 1,
        pageSize: 100,
      });
      const planExercises = response.data.Items || [];

      const exerciseRequests = planExercises.map(wpe =>
        baseApi.get(`/exercise?id=${wpe.ExerciseId}`).then(res => ({
          ...wpe,
          Exercise: res.data
        })).catch(() => wpe)
      );
      const planExercisesWithData = await Promise.all(exerciseRequests);
      setSelectedPlanExercises(planExercisesWithData);
    } catch (error) {
      setSelectedPlanExercises([]);
    }
  };

  const addWorkoutPlanButton = (
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
      onMouseOver={e => e.currentTarget.style.background = "#356ad6"}
      onMouseOut={e => e.currentTarget.style.background = "#4f8cff"}
      onClick={() => setAddModalOpen(true)}
    >
      ➕ ADD WORKOUT PLAN
    </button>
  );

  return (
    <div style={{ padding: 20 }}>
      <h2>🏋️‍♂️ MY WORKOUT PLANS</h2>
      <div className="workoutplan-table-wrapper">
        <GenericTable
          store={workoutPlanStore}
          onRowClick={handleWorkoutPlanClick}
          headerButton={addWorkoutPlanButton}
          onDeleteRow={id => openDeleteModal("workoutplan", id)}
        />
      </div>

      {currentWorkoutPlan && (
        <div>
          <div style={{
            display: "flex",
            alignItems: "center",
            justifyContent: "space-between",
            marginBottom: 12,
            gap: 16
          }}>
            <h3 style={{ margin: 0 }}>
              🏋️‍♂️ EXERCISES IN {currentWorkoutPlan.Name ? `"${currentWorkoutPlan.Name}"` : "SELECTED PLAN"}
            </h3>
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
                marginLeft: 16
              }}
              onClick={() => setAddExerciseModalOpen(true)}
            >
              ➕ ADD EXERCISE
            </button>
          </div>
          {selectedPlanExercises.length === 0 ? (
            <div style={{ textAlign: "center", marginTop: 32 }}>
              <p style={{ fontSize: "1.1em", color: "#888", marginBottom: 18 }}>
                No exercises added yet.
              </p>
            </div>
          ) : (
            <GenericTable
              items={selectedPlanExercises.map(wpe => ({
                Id: wpe.Id,
                Name: wpe.Exercise?.Name ?? "",
                Desc: wpe.Exercise?.Desc ?? "",
                Reps: wpe.Exercise?.Reps ?? "",
                Sets: wpe.Exercise?.Sets ?? "",
                RestPeriod: wpe.Exercise?.RestPeriod ?? ""
              }))}

              onDeleteRow={item => { openDeleteModal("exercise", item); }}
            />
          )}
        </div>
      )}

      {addModalOpen && (
        <AddWorkoutPlanModal
          open={addModalOpen}
          onClose={() => setAddModalOpen(false)}
          onAdded={() => workoutPlanStore.fetchAll()}
        />
      )}

      <AddExerciseToPlanModal
        open={addExerciseModalOpen}
        onClose={() => setAddExerciseModalOpen(false)}
        workoutPlanId={currentWorkoutPlan ? currentWorkoutPlan.Id : null}
        onAdded={async () => {
          setAddExerciseModalOpen(false);
          if (currentWorkoutPlan) {
            await handleWorkoutPlanClick(currentWorkoutPlan);
          }
        }}
      />

      <ConfirmDeleteModal
        open={deleteModalOpen}
        onClose={() => setDeleteModalOpen(false)}
        onConfirm={handleDeleteConfirmed}
        text={
          deleteTarget.type === "exercise"
            ? "Are you sure you want to remove this exercise from the plan?"
            : "This will remove the workout plan and all its exercises."
        }
      />
    </div>
  );
});

export default WorkoutPlanPage;
