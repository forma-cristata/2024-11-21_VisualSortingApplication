document.addEventListener('DOMContentLoaded', () =>
{
    const observer = new MutationObserver((mutationsList) =>
    {
        for (const mutation of mutationsList)
        {
            if (mutation.type === 'childList')
            {
                console.log('Child nodes changed in the app div');
                blazorLoaded();
                observer.disconnect();
                break;
            }
        }
    });

    // Start observing the app div for child node changes
    const appDiv = document.getElementById('app');
    if (appDiv)
    {
        observer.observe(appDiv, { childList: true, subtree: true });
    }
});

function blazorLoaded()
{
    setElementsMax();

    document.getElementById('elements').addEventListener('change', (e) =>
    {
        e.target.value = Math.min(e.target.value, document.documentElement.clientWidth);
        e.target.value = Math.max(e.target.value, e.target.min);
    });

    // Listen for window resize events
    window.addEventListener('resize', () =>
    {
        console.log('Window resized');
        setElementsMax();
    });
}

function setElementsMax()
{
    document.getElementById('elements').max = document.documentElement.clientWidth;
}

window.getNumberOfElements = () =>
{
    const elements = document.getElementById('elements');
    return elements.valueAsNumber;
}
