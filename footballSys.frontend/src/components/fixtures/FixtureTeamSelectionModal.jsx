import { useState, useEffect } from "react";
import "./FixtureTeamSelectionModal.css";

const FixtureTeamSelectionModal = ({ onClose, onGenerate }) => {
  const [teams, setTeams] = useState([]);
  const [selectedTeamIds, setSelectedTeamIds] = useState([]);

  useEffect(() => {
    fetch("http://localhost:5201/intro")
      .then((res) => res.json())
      .then((data) => {
        console.log("Teams fetched:", data); //! delete this later
        setTeams(data);
      })
      .catch((err) => console.error(err));
  }, []);


  //* we add ids to array here
  const handleTeamSelection = (teamId) => {
    setSelectedTeamIds((prev) =>
      prev.includes(teamId)
        ? prev.filter((id) => id !== teamId)
        : [...prev, teamId],
    );
  };


  //! adjust this
  //send selected teams to the backend
  const handleGenerateFixture = () => {
  console.log("Selected IDs before sending:", selectedTeamIds);

  if (selectedTeamIds.length < 2) {
    alert("Select at least 2 teams to generate a fixture!");
    return;
  }

  fetch("http://localhost:5201/api/fixtures/generate", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(selectedTeamIds),
  })
    .then(async (res) => {
      if (!res.ok) {
        // Try to read the response text safely
        const text = await res.text();
        throw new Error(`Server returned ${res.status}: ${text}`);
      }
      return res.json();
    })
    .then((data) => {
      console.log(`Generated output: ${JSON.stringify(data)}`); //! comment later
      onGenerate(data); //send data 
    }) //! delete below later
    .catch((err) => {
      // Now you'll see the full server error
      console.error("Fetch error:", err);
      alert("Error generating fixtures. Check console for details.");
    });
};

  //TODO: more info in selection modal.
  return (
    <div className="modal-overlay">
      <div className="modal">
        <h2>Select Teams</h2>

        <div className="team-list">
          {teams.map((team) => (
            <div key={team.id} className="team-item">
              <input
                type="checkbox"
                checked={selectedTeamIds.includes(team.id)}
                onChange={() => handleTeamSelection(team.id)}
              />
              <span>{team.teamName}</span>
            </div>
          ))}
        </div>

        <div className="modal-buttons">
          <button onClick={onClose}>Cancel</button>
          <button onClick={handleGenerateFixture}>Generate Fixture</button>
        </div>
      </div>
    </div>
  );
};

export default FixtureTeamSelectionModal;
