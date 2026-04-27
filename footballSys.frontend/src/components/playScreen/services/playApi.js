const BASE_URL = "http://localhost:5201/api";

// fetch a week
export const fetchWeekApi = async (fixtureId, weekNumber) => {
  const res = await fetch(
    `${BASE_URL}/fixtures/${fixtureId}/weeks/${weekNumber}`
  );
  return await res.json();
};

// play one week
export const playWeekApi = async (fixtureId, weekNumber) => {
  const res = await fetch(
    `${BASE_URL}/simulation/play-week/${fixtureId}/${weekNumber}`,
    {
      method: "POST",
    }
  );
  return await res.json();
};

// play all
export const playAllApi = async (fixtureId) => {
  const res = await fetch(
    `${BASE_URL}/simulation/play-all/${fixtureId}`,
    {
      method: "POST",
    }
  );
  return await res.json();
};

// league table
export const fetchLeagueTableApi = async (fixtureId, week) => {
  const res = await fetch(
    `${BASE_URL}/simulation/${fixtureId}/table?week=${week}`
  );
  return await res.json();
};