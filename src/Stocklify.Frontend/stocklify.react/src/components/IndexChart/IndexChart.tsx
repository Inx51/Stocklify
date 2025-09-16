import React, { useEffect, useState } from 'react';
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend,
} from 'chart.js';
import { Line } from 'react-chartjs-2';
import { faker } from '@faker-js/faker';


ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend
);

export const options = {
  pointStyle: false,
  responsive: true,
  plugins: {
    legend: {
      display: false
    },
    title: {
      display: false
    },
  },
  scales: {
    y: {
      display: false
    },
    x: {
      display: false
    }
  }
};

const longDataArray = Array.from({ length: 100 }, () => 
  faker.number.int({ min: 0, max: 1000 })
);

let labels = longDataArray.map((_, index) => index.toString());

export const data = {
  labels: labels,
  datasets: [
    {
      data: longDataArray,
      borderWidth: 2,
      borderColor: 'rgb(255, 99, 132)',
      backgroundColor: 'rgba(255, 99, 132, 0.5)',
    }
  ],
};

export function IndexChart({values}: {values:number[]}) {

  const [data, setData] = useState({} as any);

  useEffect(() => {
    labels = values.map((_, index) => index.toString());
    data.datasets[0].data = values;
    setData(data);
  }, [values]);

  return <Line options={options} data={data} height={300} />;
}
