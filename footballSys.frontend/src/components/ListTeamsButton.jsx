import { useState } from "react";
import { getTeams } from "../api/teamApi.js";
import TeamsModal from "./teams/teamModal.jsx";
import { deleteTeam } from "../api/teamApi.js";

const ListTeamsButton = () => {
  const [teams, setTeams] = useState([]);
  const [isOpen, setIsOpen] = useState(false);

  const handleClick = async () => {
    const data = await getTeams();
    setTeams(data);
    setIsOpen(true);
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
      <button onClick={handleClick}>Show Teams</button>

      {isOpen && (
  <TeamsModal title="Teams" onClose={() => setIsOpen(false)}>
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
          </tr>
        ))}
      </tbody>
    </table>
  </TeamsModal>
)}
    </>
  );
};

export default ListTeamsButton;