import React, { useMemo } from 'react';
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend,
  Filler
} from 'chart.js';
import { Line } from 'react-chartjs-2';


ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend,
  Filler
);

interface IndexChartProps {
  values: number[];
  isPositive: boolean;
}

export function IndexChart({ values, isPositive }: IndexChartProps) {

  // Memoize chart data to prevent unnecessary re-calculations
  const chartData = useMemo(() => ({
    labels: values.map((_, index) => index.toString()), // Labels go here, not in dataset
    datasets: [
      {
        data: values,
        borderWidth: 2,
        borderColor: isPositive ? 'rgb(16, 185, 129)' : 'rgb(239, 68, 68)',
        backgroundColor: isPositive ? 'rgba(16, 185, 129, 0.1)' : 'rgba(239, 68, 68, 0.1)',
        fill: true,
        tension: 0.1,
        pointRadius: 0,
        pointHoverRadius: 0,
      }
    ],
  }), [values, isPositive]);

  const options = useMemo(() => ({
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        display: false
      },
      title: {
        display: false
      },
      tooltip: {
        enabled: false,
        titleDisplay: false
      },
    },
    scales: {
      y: {
        display: false,
        beginAtZero: false
      },
      x: {
        display: false
      }
    },
    elements: {
      point: {
        radius: 0
      }
    },
    animation: {
      duration: 0
    },
    interaction: {
      intersect: false,
    },
  }), []);

  // Handle empty values array after hooks are called
  if (!values || values.length === 0) {
    return <div>No data available</div>;
  }

  return (
      <Line options={options} data={chartData} />
  );
}
