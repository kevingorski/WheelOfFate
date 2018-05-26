import checkFetchResponse from './checkFetchResponse';

const getRecentSchedules = () =>
  fetch('/api/supportschedules')
    .then(checkFetchResponse)
    .then(response => response.json());

const generateSchedule = (supportScheduleSpecification) => 
  fetch('/api/supportschedules', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(supportScheduleSpecification)
    })
    .then(checkFetchResponse)
    .then(response => response.json());

export {
  getRecentSchedules,
  generateSchedule
};