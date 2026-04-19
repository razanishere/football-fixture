import { useState } from "react";
import { getTeams } from "../api/teamApi.js";
import TeamsModal from "./teams/teamModal.jsx";
import { deleteTeam, updateTeam} from "../api/teamApi.js";
import EditTeamModal from "./teams/EditTeamModal.jsx"; 
import AddTeamForm from "./addTeamForm.jsx";





const ListTeamsButton = () => {
  const [teams, setTeams] = useState([]);
  const [isOpen, setIsOpen] = useState(false);

  const [editingTeam, setEditingTeam] = useState(null); // stores the team being edited
  const [isEditOpen, setIsEditOpen] = useState(false); // controls the edit modal

  const [isAddOpen, setIsAddOpen] = useState(false);

  const handleClick = async () => {
    const data = await getTeams();
    setTeams(data);
    setIsOpen(true);
  };

  const openEditModal = (team) => {
  setEditingTeam(team);
  setIsEditOpen(true);
};


const handleSaveEdit = async (updatedTeam, file) => {
  
  await updateTeam(editingTeam.id, updatedTeam, file);
  const freshTeams = await getTeams();
  setTeams(freshTeams);
  setIsEditOpen(false);
};



  //* Delete action

  const handleDelete = async (id) => {

    //TODO: replace this alert with a pop up.
  const confirmed = window.confirm(
    "Are you sure you want to delete this team?"
  );

  if (!confirmed) return;
  await deleteTeam(id);

  setTeams((prevTeams) =>
    prevTeams.filter((team) => team.id !== id)
  );
};

  return (
    <>
      <button onClick={handleClick}>Teams</button>

      {isOpen && (
  <TeamsModal title="Teams" onClose={() => setIsOpen(false)}>

        <button onClick={() => setIsAddOpen(true)}>
    Add Team
  </button>


    <table>
      <thead>
        <tr>
          <th>#</th>
          <th>Name</th>
          <th>Year</th>
          <th>Color 1</th>
          <th>Color 2</th>
          <th>Logo</th>
          <th>Actions</th>
        </tr>
      </thead>

      <tbody>
        {teams.map((team, index) => (
          <tr key={team.id}>
            <td>{index + 1}</td>
            <td>{team.teamName}</td>
            <td>{team.yearEstablished}</td>
            <td>{team.teamColor1}</td>
            <td>{team.teamColor2}</td>
            <td>
              <img
                src={`http://localhost:5201/${team.logoURL}`}
                alt={team.teamName}
                className="team-logo"
              />
            </td>
            <td>
                 <button onClick={() => handleDelete(team.id)}>
                 Delete
                 </button>
            </td>
            <td>
              <button onClick={() => openEditModal(team)}>
                Edit
              </button>
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  </TeamsModal>
)}


{isEditOpen && editingTeam && (
  <EditTeamModal
    team={editingTeam}
    onClose={() => setIsEditOpen(false)}
    onSave={handleSaveEdit}
  />
)}

{isAddOpen && (
  <TeamsModal title="Add New Team" onClose={() => setIsAddOpen(false)}>
    <AddTeamForm onClose={() => setIsAddOpen(false)} />
  </TeamsModal>
)}
    </>
  );
};

export default ListTeamsButton;