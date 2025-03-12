function renderChart(chartId, labels, data) {
    var ctx = document.getElementById(chartId).getContext("2d");
    new Chart(ctx, {
        type: "bar",
        data: {
            labels: labels,
            datasets: [{
                label: "Oylik xarajatlar",
                data: data,
                backgroundColor: "rgba(54, 162, 235, 0.7)",
                borderColor: "rgba(54, 162, 235, 1)",
                borderWidth: 1,
                borderRadius: 10, // Smooth bars
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        font: { size: 14 },
                        color: "#555"
                    }
                },
                x: {
                    ticks: {
                        font: { size: 14 },
                        color: "#555"
                    }
                }
            },
            plugins: {
                legend: {
                    labels: {
                        font: { size: 16 },
                        color: "#333"
                    }
                },
                tooltip: {
                    backgroundColor: "rgba(0,0,0,0.8)",
                    titleFont: { size: 16 },
                    bodyFont: { size: 14 },
                }
            }
        }
    });
}
