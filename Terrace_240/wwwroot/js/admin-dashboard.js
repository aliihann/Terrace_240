// Show toast notification
function showToast(message, type = 'success') {
    const toast = document.getElementById('toast');
    toast.textContent = message;
    toast.className = `toast ${type} show`;

    setTimeout(() => {
        toast.classList.remove('show');
    }, 3000);
}

// Update statistics
function updateStatistics() {
    const totalMovies = movies.length;
    const avgRating = totalMovies > 0 ? (movies.reduce((sum, movie) => sum + movie.rating, 0) / totalMovies).toFixed(1) : '0.0';
    const avgPrice = totalMovies > 0 ? Math.round(movies.reduce((sum, movie) => sum + movie.price, 0) / totalMovies) : 0;

    document.getElementById('totalMovies').textContent = totalMovies;
    document.getElementById('avgRating').textContent = avgRating;
    document.getElementById('avgPrice').textContent = avgPrice;
}



// Show confirm dialog
function showConfirmDialog(title, message, onConfirm) {
    document.getElementById('confirmTitle').textContent = title;
    document.getElementById('confirmMessage').textContent = message;
    document.getElementById('confirmDialog').style.display = 'flex';
    document.body.style.overflow = 'hidden';

    const handleConfirm = () => {
        onConfirm();
        closeConfirmDialog();
        document.getElementById('confirmOk').removeEventListener('click', handleConfirm);
    };

    document.getElementById('confirmOk').addEventListener('click', handleConfirm);
}

// Close confirm dialog
function closeConfirmDialog() {
    document.getElementById('confirmDialog').style.display = 'none';
    document.body.style.overflow = 'auto';
}



// Event listeners
document.addEventListener('DOMContentLoaded', function () {
    renderMoviesTable();
    updateStatistics();

 

    // Reset data
    document.getElementById('resetDataBtn').addEventListener('click', function () {
        showConfirmDialog(
            'Сбросить данные',
            'Вы уверены, что хотите сбросить все данные к настройкам по умолчанию? Это действие нельзя отменить.',
            resetData
        );
    });

  

    // Cancel confirm dialog
    document.getElementById('confirmCancel').addEventListener('click', closeConfirmDialog);

    // Close dialog on backdrop click
    document.getElementById('confirmDialog').addEventListener('click', function (e) {
        if (e.target === this) {
            closeConfirmDialog();
        }
    });
});