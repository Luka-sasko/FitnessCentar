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

  const handleExerciseAdded = async () => {
    if (!currentWorkoutPlan) return;
    await handleWorkoutPlanClick(currentWorkoutPlan);
  };

  const addWorkoutPlanButton = (
    <button className="add-workoutplan-btn" onClick={() => setAddModalOpen(true)}>
      <span className="add-plus">＋</span>
      ADD WORKOUT PLAN
    </button>
  );

  const addExerciseButton = (
    <button className="add-exercise-btn" onClick={() => setAddExerciseModalOpen(true)}>
      <span className="add-plus">＋</span>
      ADD EXERCISE
    </button>
  );

  return (
    <div style={{ padding: 20 }}>
      <div
        style={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
          marginBottom: 18,
        }}
      >
        <h2 style={{ margin: 0, color: "white" }}>🏋️‍♂️ MY WORKOUT PLANS</h2>
        {addWorkoutPlanButton}
      </div>
      <div className="workoutplan-table-wrapper">
        <GenericTable
          store={workoutPlanStore}
          onRowClick={handleWorkoutPlanClick}
          onDeleteRow={id => openDeleteModal("workoutplan", id)}
        />
      </div>

      {currentWorkoutPlan && (
        <div>
          <div
            style={{
              display: "flex",
              alignItems: "center",
              justifyContent: "space-between",
              marginBottom: 12,
              gap: 16,
            }}
          >
            <h3 style={{ margin: 0 }}>
              🏋️‍♂️ EXERCISES IN {currentWorkoutPlan.Name ? `"${currentWorkoutPlan.Name}"` : "SELECTED PLAN"}
            </h3>
            {addExerciseButton}
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
                Description: wpe.Exercise?.Description ?? "",
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
        onAdded={handleExerciseAdded}
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
