if ('wakeLock' in navigator) {
    let wakeLock = null;

    const requestWakeLock = async () => {
        try {
            wakeLock = await navigator.wakeLock.request('screen');
            document.body.classList.add("wake-lock-active");
            
            wakeLock.addEventListener('release', () => {
                document.body.classList.remove("wake-lock-active");
            });
        } catch (err) {
            document.body.classList.remove("wake-lock-active");
            console.error(`Error requesting screen wake lock: ${err.name}, ${err.message}`);
        }
    }

    const releaseWakeLock = () => {
        if (wakeLock) {
            wakeLock.release();
            wakeLock = null;
        }
    }

    // Request wake lock when the PWA is in use
    requestWakeLock();

    window.addEventListener('visibilitychange', () => {
        if (document.hidden) {
            releaseWakeLock();
        } else {
            requestWakeLock();
        }
    });
}