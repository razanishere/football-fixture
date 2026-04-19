import TeamsModal from "./teamModal.jsx";

const labelStyle = {
  display: "block",
  marginBottom: "8px",
};

const EditTeamModal = ({ team, onClose, onSave }) => {
  return (
    <TeamsModal title="Edit Team" onClose={onClose}>
      <form
  onSubmit={(e) => {
    e.preventDefault();

    const updatedTeam = {
      teamName: e.target.teamName.value,
      yearEstablished: parseInt(e.target.yearEstablished.value),
      teamColor1: e.target.teamColor1.value,
      teamColor2: e.target.teamColor2.value,
    };

    const file = e.target.teamLogo.files[0];

    onSave(updatedTeam, file);
  }}
>
      
        <label style={labelStyle}>
          Name:
          <input name="teamName" defaultValue={team.teamName} />
        </label>
        <label style={labelStyle}>
          Year:
          <input
            name="yearEstablished"
            type="number"
            defaultValue={team.yearEstablished}
          />
        </label>
        <label style={labelStyle}>
          Color 1:
          <input name="teamColor1" defaultValue={team.teamColor1} />
        </label>
        <label style={labelStyle}>
          Color 2:
          <input name="teamColor2" defaultValue={team.teamColor2} />
        </label>
        
        <label style={labelStyle}>
        Logo:
        <input
            type="file"
            name="teamLogo"
            accept="image/png, image/jpeg"
        />
        </label>

        <button type="submit">Save Changes</button>
      </form>
    </TeamsModal>
  );
};

export default EditTeamModal;