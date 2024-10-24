plugin(({ addComponents }) => {
  // simplified version of https://github.com/karolis-sh/tailwind-bootstrap-grid
  const gridColumns = 12;
  const gridGutterWidth = 24;
  const generateContainer = true;
  const rtl = false;
  const columns = Array.from(Array(gridColumns), (value, index) => index + 1);
  const rowColsSteps = columns.slice(0, Math.floor(gridColumns / 2));

  // container
  if (generateContainer) {
    addComponents(
      [
        {
          '.container': {
            width: '100%',
            marginRight: 'auto',
            marginLeft: 'auto',
            paddingRight: `${gridGutterWidth / 2}px`,
            paddingLeft: `${gridGutterWidth / 2}px`,
          },
        },
      ],
    );
  }

  // row
  addComponents(
    {
      '.row': {
        display: 'flex',
        flexDirection: 'row',
        flexWrap: 'wrap',
        marginRight: `-${gridGutterWidth / 2}px`,
        marginLeft: `-${gridGutterWidth / 2}px`,
        '& > *': {
          flexShrink: 0,
          width: '100%',
          maxWidth: '100%',
          paddingRight: `${gridGutterWidth / 2}px`,
          paddingLeft: `${gridGutterWidth / 2}px`,
        },
      }
    },
  );

  // columns
  addComponents(
    [
      {
        '.col': {
          flex: '1 0 0%',
        },
        '.row-cols-auto': {
          '& > *': {
            flex: '0 0 auto',
            width: 'auto',
          },
        },
      },
      ...rowColsSteps.map((rowCol) => ({
        [`.row-cols-${rowCol}`]: {
          '& > *': {
            flex: '0 0 auto',
            width: `${100 / rowCol}%`,
          },
        },
      })),
      {
        '.col-auto': {
          flex: '0 0 auto',
          width: 'auto',
        },
      },
      ...columns.map((size) => ({
        [`.col-${size}`]: {
          flex: '0 0 auto',
          width: `${(100 / gridColumns) * size}%`,
        },
      })),
    ],
  );

  // offset
  addComponents(
    [
      ...[0, ...columns.slice(0, -1)].map((size) => {
        const margin = `${(100 / gridColumns) * size}%`;
        return rtl
          ? {
            [`[dir="ltr"] .offset-${size}`]: { marginLeft: margin },
            [`[dir="rtl"] .offset-${size}`]: { marginRight: margin },
          }
          : {
            [`.offset-${size}`]: { marginLeft: margin },
          };
      }),
    ],
  );
})
