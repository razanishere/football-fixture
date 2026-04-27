const LeagueTableModal = ({
  show,
  onClose,
  leagueTable,
}) => {
  if (!show) return null;

  const sortedTable = [...leagueTable].sort((a, b) => {
    if (b.points !== a.points) return b.points - a.points;
    if (b.goalDifference !== a.goalDifference)
      return b.goalDifference - a.goalDifference;
    return b.goalsScored - a.goalsScored;
  });

  return (
    <div
      style={{
        position: "fixed",
        top: 0,
        left: 0,
        width: "100%",
        height: "100%",
        backgroundColor: "rgba(0,0,0,0.7)",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
      }}
    >
      <div
        style={{
          background: "white",
          padding: "20px",
          borderRadius: "10px",
          width: "500px",
        }}
      >
        <div
          style={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "center",
          }}
        >
          <h2>League Table</h2>

          <button
            onClick={onClose}
            style={{
              background: "transparent",
              border: "none",
              fontSize: "20px",
              cursor: "pointer",
            }}
          >
            ✖
          </button>
        </div>

        <table style={{ width: "100%", borderCollapse: "collapse" }}>
          <thead>
            <tr style={{ borderBottom: "2px solid black" }}>
              <th>#</th>
              <th>Team</th>
              <th>P</th>
              <th>W</th>
              <th>D</th>
              <th>L</th>
              <th>GS</th>
              <th>GC</th>
              <th>Pts</th>
            </tr>
          </thead>

          <tbody>
            {sortedTable.map((team, index) => (
              <tr
                key={team.teamId}
                style={{
                  textAlign: "center",
                  backgroundColor:
                    index === 0 ? "#ffec3d" : "transparent",
                  fontWeight: index === 0 ? "bold" : "normal",
                }}
              >
                <td>{index + 1}</td>
                <td style={{ textAlign: "left" }}>{team.teamName}</td>
                <td>{team.played}</td>
                <td>{team.wins}</td>
                <td>{team.draws}</td>
                <td>{team.losses}</td>
                <td>{team.goalsScored}</td>
                <td>{team.goalsConceded}</td>
                <td>
                  <strong>{team.points}</strong>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default LeagueTableModal;