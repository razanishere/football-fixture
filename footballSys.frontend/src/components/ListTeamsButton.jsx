// import { useState } from "react";
// import { getTeams } from "../api/teamApi";

// const ListTeamsButton = () => {
//   const [teams, setTeams] = useState([]);

//   const handleClick = async () => {
//     const data = await getTeams();
//     console.log(data); //! check if api working, delete later
//     setTeams(data);
//   };

//   return (
//     <div>
//       <button onClick={handleClick}>Show Teams</button>

//       {teams.length > 0 && (
//         <table border="1">
//           <thead>
//             <tr>
//               <th>ID</th>
//               <th>Name</th>
//               <th>Year</th>
//               <th>Color 1</th>
//               <th>Color 2</th>
//               <th>Logo</th>
//             </tr>
//           </thead>

//           <tbody>
//   {teams.map((team, index) => (
//     <tr key={team.id}>
//       <td>{index + 1}</td>
//       <td>{team.teamName}</td>
//       <td>{team.yearEstablished}</td>
//       <td>{team.teamColor1}</td>
//       <td>{team.teamColor2}</td>
//       <td>
//         <img
//           src={`http://localhost:5201/${team.logoURL}`}
//           alt={team.teamName}
//         />

//       </td>
//     </tr>
//   ))}
// </tbody>

//         </table>
//       )}
//     </div>
//   );
// };

// export default ListTeamsButton;


import { useState } from "react";
import { getTeams } from "../api/teamApi";
import TeamsModal from "./teams/teamModal.jsx";

const ListTeamsButton = () => {
  const [teams, setTeams] = useState([]);
  const [isOpen, setIsOpen] = useState(false);

  const handleClick = async () => {
    const data = await getTeams();
    setTeams(data);
    setIsOpen(true);
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