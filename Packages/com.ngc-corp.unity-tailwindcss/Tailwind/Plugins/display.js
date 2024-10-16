plugin(function ({ addUtilities }) {
  const displayUtilities = {
    '.none': { display: 'none' },
    '.flex': { display: 'flex' },
  };

  // Add the utilities (responsive, hover, focus, etc.)
  addUtilities(displayUtilities, ['responsive', 'hover', 'focus', 'active']);
})
