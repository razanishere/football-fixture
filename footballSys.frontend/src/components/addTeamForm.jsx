// //* i make the form inside the button first and then i will call it through the button

// import { useState } from "react";
// import { createTeam } from "../api/teamApi";

// const AddTeamForm = () => {
//   const [teamName, setTeamName] = useState("");
//   const [yearEstablished, setYearEstablished] = useState("");
//   const [teamColor1, setTeamColor1] = useState("");
//   const [teamColor2, setTeamColor2] = useState("");
//   const [logo, setLogo] = useState(null);

//   const handleSubmit = async (e) => {
//     e.preventDefault();

//     if (!logo) {
//       alert("Please upload a logo");
//       return;
//     }

//     const newTeam = {
//       teamName,
//       yearEstablished,
//       teamColor1,
//       teamColor2,
//       logo, // File object
//     };

//     await createTeam(newTeam);
//     alert("Team created successfully");
//   };

//   return (
//     <form onSubmit={handleSubmit}>
//       <div>
//         <label>Team Name</label>
//         <input
//           type="text"
//           value={teamName}
//           onChange={(e) => setTeamName(e.target.value)}
//           required
//         />
//       </div>

//       <div>
//         <label>Year Established</label>
//         <input
//           type="number"
//           value={yearEstablished}
//           onChange={(e) => setYearEstablished(e.target.value)}
//           required
//         />
//       </div>

//       <div>
//         <label>Primary Color</label>
//         <input
//           type="text"
//           value={teamColor1}
//           onChange={(e) => setTeamColor1(e.target.value)}
//           required
//         />
//       </div>

//       <div>
//         <label>Secondary Color</label>
//         <input
//           type="text"
//           value={teamColor2}
//           onChange={(e) => setTeamColor2(e.target.value)}
//           required
//         />
//       </div>

//       <div>
//         <label>Team Logo</label>
//         <input
//           type="file"
//           accept="image/*"
//           onChange={(e) => setLogo(e.target.files[0])}
//           required
//         />
//       </div>

//       <button type="submit">Add Team</button>
//     </form>
//   );
// };

// export default AddTeamForm;

import { useState } from "react";
import { createTeam } from "../api/teamApi";

const AddTeamForm = ({ onClose }) => {
  const [teamName, setTeamName] = useState("");
  const [yearEstablished, setYearEstablished] = useState("");
  const [teamColor1, setTeamColor1] = useState("");
  const [teamColor2, setTeamColor2] = useState("");
  const [logo, setLogo] = useState(null);

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!logo) {
      alert("Please upload a logo");
      return;
    }

    const newTeam = {
      teamName,
      yearEstablished,
      teamColor1,
      teamColor2,
      logo,
    };

    await createTeam(newTeam);
    alert("Team created successfully");
    onClose();
  };

  return (
    <form className="form">
      <div className="form-group">
        <label>Team Name</label>
        <input
          type="text"
          value={teamName}
          onChange={(e) => setTeamName(e.target.value)}
          required
        />
      </div>

      <div className="form-group">
        <label>Year Established</label>
        <input
          type="number"
          value={yearEstablished}
          onChange={(e) => setYearEstablished(e.target.value)}
          required
        />
      </div>

      <div className="form-row">
        <div className="form-group">
          <label>Primary Color</label>
          <input
            type="text"
            value={teamColor1}
            onChange={(e) => setTeamColor1(e.target.value)}
            required
          />
        </div>

        <div className="form-group">
          <label>Secondary Color</label>
          <input
            type="text"
            value={teamColor2}
            onChange={(e) => setTeamColor2(e.target.value)}
            required
          />
        </div>
      </div>

      <div className="form-group">
        <label>Team Logo</label>
        <input
          type="file"
          accept="image/*"
          onChange={(e) => setLogo(e.target.files[0])}
          required
        />
      </div>

      <div className="form-actions">
        <button type="submit">Add Team</button>
        <button type="button" onClick={onClose}>
          Cancel
        </button>
      </div>
    </form>
  );
};

export default AddTeamForm;