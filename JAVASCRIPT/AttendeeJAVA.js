// Base URL for the API
const APIBaseURL = 'http://localhost:5000/api/attendees';

// Fetch all attendees and display in the table
async function fetchAttendees() {
    try {
        const response = await fetch(APIBaseURL);
        const attendees = await response.json();
        const tableBody = document.getElementById('attendees-table').querySelector('tbody');
        tableBody.innerHTML = '';

        attendees.forEach(attendee => {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${attendee.name}</td>
                <td>${attendee.email}</td>
                <td>${attendee.event}</td>
                <td>
                    <button onclick="deleteAttendee('${attendee.id}')">Delete</button>
                    <button onclick="editAttendee('${attendee.id}')">Edit</button>
                </td>
            `;
            tableBody.appendChild(row);
        });
    } catch (error) {
        console.error('Error fetching attendees:', error);
    }
}

// Add a new attendee
async function addAttendee(event) {
    event.preventDefault();
    const name = document.getElementById('attendee-name').value;
    const email = document.getElementById('attendee-email').value;
    const eventName = document.getElementById('attendee-event').value;

    try {
        await fetch(APIBaseURL, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ name, email, event: eventName })
        });
        fetchAttendees();
        event.target.reset();
    } catch (error) {
        console.error('Error adding attendee:', error);
    }
}

// Delete an attendee
async function deleteAttendee(id) {
    try {
        await fetch(`${APIBaseURL}/${id}`, { method: 'DELETE' });
        fetchAttendees();
    } catch (error) {
        console.error('Error deleting attendee:', error);
    }
}

// Update an attendee
async function updateAttendee(id, updatedData) {
    try {
        await fetch(`${APIBaseURL}/${id}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(updatedData)
        });
        fetchAttendees(); // Refresh attendee list
    } catch (error) {
        console.error('Error updating attendee:', error);
    }
}

// Edit an attendee
function editAttendee(id) {
    const name = prompt('Enter new name:');
    const email = prompt('Enter new email:');
    const eventName = prompt('Enter new event name:');

    if (name && email && eventName) {
        updateAttendee(id, { name, email, event: eventName });
    } else {
        alert('All fields are required for updating!');
    }
}

// Event listener for the add attendee form
document.getElementById('add-attendee-form').addEventListener('submit', addAttendee);

// Fetch attendees on page load
window.onload = fetchAttendees;
