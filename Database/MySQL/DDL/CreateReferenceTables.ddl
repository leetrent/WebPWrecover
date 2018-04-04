--USE LMS;
CREATE TABLE `Agency` (
  `AgencyName` varchar(250) NOT NULL,
  `AgencyCode` smallint NOT NULL,
  PRIMARY KEY (`AgencyName`, `AgencyCode`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;