import { useState, useEffect } from "react";
import "./FixtureTeamSelectionModal.css";

const FixtureTeamSelectionModal = ({ onClose, onGenerate }) => {
  const [teams, setTeams] = useState([]);
  const [selectedTeamIds, setSelectedTeamIds] = useState([]);

  const [fixtureNameInput, setFixtureNameInput] = useState("");

  //special mode
  const [isSpecialMode, setIsSpecialMode] = useState(false);

  useEffect(() => {
    fetch("http://localhost:5201/intro")
      .then((res) => res.json())
      .then((data) => {
        console.log("Teams fetched:", data);
        setTeams(data);
      })
      .catch((err) => console.error(err));
  }, []);

  const handleTeamSelection = (teamId) => {
    setSelectedTeamIds((prev) =>
      prev.includes(teamId)
        ? prev.filter((id) => id !== teamId)
        : [...prev, teamId],
    );
  };

  const handleGenerateFixture = () => {
    console.log("Selected IDs before sending:", selectedTeamIds);

    if (selectedTeamIds.length < 2) {
      alert("Select at least 2 teams to generate a fixture!");
      return;
    }

    console.log("FINAL PAYLOAD:", {
      teamIds: selectedTeamIds,
      fixtureName: fixtureNameInput,
    });

    fetch("http://localhost:5201/api/fixtures/generate", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        teamIds: [...selectedTeamIds].map(Number),
        fixtureName: fixtureNameInput ?? "",
        isSpecialMode: isSpecialMode, 
      }),
    })
      .then(async (res) => {
        if (!res.ok) {
          const text = await res.text();
          throw new Error(`Server returned ${res.status}: ${text}`);
        }
        return res.json();
      })
      .then((data) => {
        console.log(`Generated output: ${JSON.stringify(data)}`);
        onGenerate(data);
      })
      .catch((err) => {
        console.error("Fetch error:", err);
        alert("Error generating fixtures. Check console for details.");
      });
  };

  return (
    <div className="modal-overlay">
      <div className="modal">
        <h2>Select Teams</h2>

        <input
          type="text"
          placeholder="Enter fixture name"
          value={fixtureNameInput}
          onChange={(e) => setFixtureNameInput(e.target.value)}
        />

        <div className="team-list">
          {teams.map((team) => (
            <div key={team.id} className="team-item">
              <input
                type="checkbox"
                checked={selectedTeamIds.includes(Number(team.id))}
                onChange={() => handleTeamSelection(Number(team.id))}
              />
              <span>{team.teamName}</span>
            </div>
          ))}
        </div>

        <div className="modal-buttons">
          <label>
            <input
              type="checkbox"
              checked={isSpecialMode}
              onChange={() => setIsSpecialMode(!isSpecialMode)}
            />
            Special Mode
          </label>
          <button onClick={onClose}>Cancel</button>
          <button onClick={handleGenerateFixture}>Generate Fixture</button>
        </div>
      </div>
    </div>
  );
};

export default FixtureTeamSelectionModal;
