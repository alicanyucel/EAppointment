import http from 'k6/http';
import { check, sleep } from 'k6';

// Spike test - sudden increase in load
export const options = {
  stages: [
    { duration: '10s', target: 10 },    // Below normal load
    { duration: '10s', target: 500 },   // Spike to 500 users
    { duration: '30s', target: 500 },   // Stay at 500 for 30 seconds
    { duration: '10s', target: 10 },    // Scale down
    { duration: '10s', target: 0 },     // Recovery
  ],
};

const BASE_URL = __ENV.BASE_URL || 'http://localhost:5000';

export default function () {
  const response = http.get(`${BASE_URL}/api/doctors/getall`);
  
  check(response, {
    'status is 200': (r) => r.status === 200,
    'response time < 2000ms': (r) => r.timings.duration < 2000,
  });

  sleep(1);
}
