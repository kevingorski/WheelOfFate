import React from 'react';

const SupportSchedule = (props) => {
  return (props.dailySchedules && 
    <section>
      <h1>Schedule</h1>
      <table>
        <thead>
          <tr>
            <th>Day</th>
            <th>Engineers</th>
          </tr>
        </thead>
        <tbody>
          {props.dailySchedules.map(dailySchedule => 
          <tr key={dailySchedule.date}>
            <td>{dailySchedule.date}</td>
            <td>
              <ol>
                {dailySchedule.engineers.map(engineer =>
                  <li key={engineer.id}>{engineer.firstName}&nbsp;{engineer.lastName}</li>
                )}
              </ol>
            </td>
          </tr>
          )}
        </tbody>
      </table>
    </section>
  );
};

export default SupportSchedule;